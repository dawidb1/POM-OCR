/*
1. MAIN FILE
    pobierz plik
    *wyślij na serwer
    wyświetl plik
2. PICTURES

3. TEXT IMAGES
4. DISPLAY

 */

// [v 0.0.1]

//MAIN FILE
// url loadFile(string path)
// {
//     bool isJpg(string path);
//     //*url copyToSerwer() //na jakiś hosting
// }
Image loadFile()
{
    bool isJpg(Image mainImage);
}

void displayFile(mainImage);

//PICTURES
//do wyboru OR
//znalezienie tych obrazów i co z nimi w obrazie głównym? zamieniać na białe?'

//DANIEL
(
void onButtonSelectPicturesClick(Image file){
    Field selectPicturesInImage(file);
    List<image> copyPictureToList(file, field);
}
OR
List<Image>,List<field> detectPictures(Image file);
);

//MACIEK
void changeDetectedToWhite(Image file, List<Field> fieldList)

//DAWID
//TEXT IMAGES
List<Image> splitTextImages(Image mainImage){ 
    MSER();
}

List<string> OCR(textImages);

//DISPLAY
void displayResults(texts, pictures)
{
    display(text);
    display(pictures);
    // do v2: wycięte miejsca znakowane kolorem do lokalizacji zdjęć
}

//POMOCNICZE
struct Field{
    int x1, x2, y1, y2;
}

// OGRANICZENIA WERSJI:
// wszystkie obrazy ładujemy na końcu strony
// może wykrywać tekst w obrazach
// nie formatuje tekstu tylko robi puste miejsca