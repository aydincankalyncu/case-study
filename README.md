# OCR parser

Bu proje, bir `response.json` dosyasını okuyarak OCR (Optical Character Recognition) verisini işler ve satırlara ayrılmış anlamlı metinler üretir.

## Uygulama Süreci

### 1. JSON Verisini Okuma
- Uygulama, `response.json` dosyasını okur.
- İçerik, `OcrItem` nesnesine deserialize (cast) edilir.

### 2. Geçersiz Objelerin Atlanması
- Dosya içeriğinde OCR tarafından tespit edilen, tüm sayfayı temsil eden (veya geçersiz) genel bir obje bulunabilir.
- Bu tür objeler analizden önce filtrelenir ve atlanır.

### 3. Word Nesnelerinin Oluşturulması
- Her bir `OcrItem` nesnesinden bir `Word` nesnesi oluşturulur.
- Kelimenin merkez koordinatları, `Vertices` dizisindeki x ve y değerlerinin ortalaması alınarak hesaplanır:


### 4. Kelimeleri Satırlara Ayırma
- Tüm `Word` nesneleri, `CenterY` değerlerine göre sıralanır.
- Satırlar `List<Word>` şeklinde temsil edilir.
- Her kelime, hali hazırda bulunan satırların ilkiyle `Y` farkı belirli bir tolerans (örneğin 10-15 px) dahilindeyse o satıra eklenir.
- Aksi halde yeni bir satır oluşturulur.

### 5. Sonuçların Üretilmesi
- Satırlar yukarıdan aşağıya doğru (artan `CenterY`'ye göre) sıralanır.
- Her satırdaki kelimeler soldan sağa doğru (`CenterX`'e göre) sıralanır.
- Her satır bir `line` objesi olarak temsil edilir ve konsola/loga yazdırılır.


# Promotion Code Generator

Bu proje belirlenmiş karakter setinden rastgele, tahmin edilmesi zor olan bir rastgele kod üretme için tasarlanmıştır.

## Genel Yapı

8 karakterlik kod iki parçaya bölünür. 7 karakter gövde 1 karakter ise kodun doğruluğunu test etmek için kullanılır.

### Kodun Üretimi
 - Kodun ilk 7 karakteri için `RandomNumberGenerator` kullanılır.
 - 7 karakterlik bir byte array tanımlanır.
 - `RandomNumberGenerator`, bu byte array'in içini rastgele sayılar ile doldurur.
 - Her bir byte, rasgele üretilen sayılar ile birlikte karakter setine göre modlanarak karakter setinden bu mod işlemine göre indeks alınır ve set edilir.

### Checksum Hesaplama

- Üretilen gövde kodunu alıp baytlara çevirip, sonra hash'ini alırız.
- ChecksumIndex hesaplamak için hash içerisindeki 16 bitlik bir sayı oluşturulur ve karakter seti uzunluğuna göre mod alınır.
- Elde edilen değer karakter setindeki indeksten alınarak checksum karakteri elde edilir.

### Validasyon

- Gelen 8 hanelik kodun ilk 7 karakteri alınır ve bu gövdeden tekrardan bir checksum hesaplama işlemi yapılır.
- Elde edilen değer, gelen koddaki son karakter ile eşleşirse kod geçerli olur.
