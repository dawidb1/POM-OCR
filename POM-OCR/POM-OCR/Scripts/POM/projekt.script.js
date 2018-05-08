var image = document.getElementById('image');
var CropList = new Array();

var cropper = new Cropper(image, {
	aspectRatio: NaN,
	zoomable: false,
	autoCrop: false,
	scalable: false,
	checkCrossOrigin: false,
	background: true,
	movable: false,
	crop: function (event) {
		console.log(event.detail.x);
		console.log(event.detail.y);
		console.log(event.detail.width);
		console.log(event.detail.height);
		console.log(event.detail.rotate);
		console.log(event.detail.scaleX);
		console.log(event.detail.scaleY);
	}
});

window.onload = $("#submitOrDeny").hide();

var globX, globY;

var imagex0;
var imagey0;

image.addEventListener('cropend', function (event) {
	$(document).on("click", function (event) {
		globX = event.pageX;
		globY = event.pageY;
	});

	$("#submitOrDeny").show();

	console.log(event.detail.originalEvent);
	console.log(event.detail.action);
});

    
$('#makeOcr').click(function () {
       
	$.ajax({
		type: "POST",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		url:  "_OcrImage",
		data: JSON.stringify(CropList),
		success: function (url) {
			$.post(url.Url, function (partial) {
                $("#result-div").html(partial);
                $("#result_child").append(`<p class="text_result">${url.Data}</p>`);
                resultViewInit();
			});
		},
		error: function (data) {
			alert('error!');
		},
	});
        
});

var THRESHOLD, numberInt;
function resultViewInit() {
    THRESHOLD = 2;

    var size = $('p').css('font-size');
    var number = size.substring(0, 2);
    numberInt = parseInt(number);

    $('#font_up').click(fontUp);
    $('#font_down').click(fontDown);
    $("#hide_file").click(hideFile);

    hideFile();
};

function hideFile() {
    $('.load-image').toggle();
    if ($('.load-image').is(":hidden")) {
        $('#hide_file').html('Show file');
        $(".pointer").hide();
    }
    else {
        $('#hide_file').html('Hide file');
        $(".pointer").show();
    }
 
};
function fontUp() {
    numberInt += THRESHOLD;
    $('p.text_result').css('font-size', numberInt);
}
function fontDown() {
    numberInt -= THRESHOLD;
    $('p.text_result').css('font-size', numberInt);
}


$("#submit").click(function () {
	cropper.crop();
	var data = cropper.getData(true);

	$("#submitOrDeny").hide();

	var image = cropper.getData(true);
	var originalImage = cropper.getImageData();

	var scale = originalImage.width / originalImage.naturalWidth;

	var cssLeftPoint = (globX - scale * image.width);
	var cssTopPoint = (globY - scale * image.height);

	var sizeX = scale * image.width;
	var sizeY = scale * image.height;

	var color = '#00ff00';
	$("body").append(
		$('<div class="pointer"></div>')
			.css('position', 'absolute')
			.css('top', cssTopPoint + 'px')
			.css('left', cssLeftPoint + 'px')
			.css('width', sizeX)
			.css('height', sizeY)
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