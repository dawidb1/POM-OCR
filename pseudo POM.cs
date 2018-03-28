// [v0.0.1]

/*
1. MAIN FILE
    pobierz plik - ok
    wyświetl plik - ok 
2. PICTURES - Daniel, Maciek
3. TEXT IMAGES - Dawid
4. DISPLAY - 
 */

#region MAIN FILE
Image loadFile()
{
    bool isJpg(Image mainImage);
}

void displayFile(mainImage);
#endregion

#region PICTURES

//DANIEL

void onButtonSelectPicturesClick(Image file){
    Field selectPicturesInImage(file);
    List<image> copyPictureToList(file, field);
}
OR
List<Image>,List<field> detectPictures(Image file);


//MACIEK
void changeDetectedToWhite(Image file, List<Field> fieldList)
#endregion

#region TEXT IMAGES
//DAWID
List<Image> splitTextImages(Image mainImage){ 
    MSER();
}

List<string> OCR(textImages);
}
#endregion

#region DISPLAY
void displayResults(texts, pictures)
{
    display(text);
    display(pictures);
    // do v2: wycięte miejsca znakowane kolorem do lokalizacji zdjęć
}
#endregion

//POMOCNICZE
struct Field{
    int x1, x2, y1, y2;
}

// OGRANICZENIA WERSJI:
// wszystkie obrazy ładujemy na końcu strony
// nie formatuje tekstu tylko go oddziela