var image = document.getElementById('image');
var CropList = new Array();

var cropper = new Cropper(image, {
	aspectRatio: NaN,
	zoomable: false,
    autoCrop: false,
    scalable: true,
    checkCrossOrigin: false,
    background: true,
    movable: false,
	crop: function (event) {
		//console.log(event.detail.x);
		//console.log(event.detail.y);
		//console.log(event.detail.width);
		//console.log(event.detail.height);
		//console.log(event.detail.rotate);
		//console.log(event.detail.scaleX);
		//console.log(event.detail.scaleY);
	}
});

window.onload = function() {
    $("#submitOrDeny").hide();
    $body = $("body");
    $(document).on({
        ajaxStart: function () {
            $body.addClass("loading");
        },
        ajaxStop: function () { $body.removeClass("loading"); }
    });
}
$(window).resize(function () {
    console.log("resized");
    $('.pointer').remove();
});

var globX, globY;

var imagex0;
var imagey0;

image.addEventListener('cropend', function (event) {
	$(document).on("click", function (event) {
		globX = event.pageX;
		globY = event.pageY;
	});

	$("#submitOrDeny").show();
});
    
$('#makeOcr').click(function () {
    $.post({
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		url:  "_OcrImage",
		data: JSON.stringify(CropList),
		success: function (data) {
			$.post(data.Url, function (partial) {
                $("#result-div").html(partial);
                $("#text_result").append(data.Text);
               
                var quill = new Quill('#text_result', {
                    theme: 'snow'
                });

                $("#to_html").click(function () {
                    var text = quill.root.innerHTML;
                    $('div.ql-toolbar').hide();
                    $("#text_result").hide();
                    $('#html_text').append(text);
                });
                resultViewInit();
			});
		},
		error: function (data) {
			alert('error!');
		},
	});
        
});

var FONT_STEP, numberInt;
function resultViewInit() {
    FONT_STEP = 2;

    var size = $('p').css('font-size');
    var number = size.substring(0, 2);
    numberInt = parseInt(number);

    $('#font_up').click(fontUp);
    $('#font_down').click(fontDown);
    $("#show_file").click(hideFile);
    $("#hide_file").click(hideFile);

    hideFile();
};

function hideFile() {
    $('.load-image').toggle();
    if ($('.load-image').is(":hidden")) {
        $('#hide_file').hide();
        $('#show_file').show();
        $(".pointer").hide();
    }
    else {
        $('#hide_file').show();
        $('#show_file').hide();
        $(".pointer").show();
    }
 
};
function fontUp() {
    numberInt += FONT_STEP;
    $('#html_text > *').css('font-size', numberInt);
}
function fontDown() {
    numberInt -= FONT_STEP;
    $('#html_text > *').css('font-size', numberInt);
}


$("#submit").click(function () {
	cropper.crop();
	var data = cropper.getData(true);

	$("#submitOrDeny").hide();
	var image = cropper.getData(true);

    var container = cropper.getContainerData();
    var canvas = cropper.getCanvasData();

    var offsetLeft = (container.width - canvas.width) / 2;
    var offsetTop = (container.height - canvas.height) / 2;
    console.log("blo");
    console.log("bla: l "+ offsetLeft);
    console.log("bla: top : " + offsetTop);

    var cropBox = cropper.getCropBoxData();
    var left = cropBox.left + offsetLeft;
    var top = cropBox.top + offsetTop;
    var height = cropBox.height;
    var width = cropBox.width;

	var color = '#00ff00';
    $("#image_div").append(
		$('<div class="pointer"></div>')
			.css('position', 'absolute')
            .css('top', top + 'px')
            .css('left', left + 'px')
            .css('width', width)
            .css('height', height)
			.css('background-color', color)
			.css('opacity', 0.5)
	);

	pushToCropList(image.x, image.y, image.width, image.height);
	cropper.clear();
});

$("#deny").click(function () {
	cropper.clear();
	$("#submitOrDeny").hide();
});

$('#refresh').click(function () {
    CropList = new Array();
    $('div.pointer').remove();
});

function pushToCropList(left, top, width, height) {

	var point = {
		X: left,
		Y: top,
		Width: width,
		Height: height
	};
	CropList.push(point);
}