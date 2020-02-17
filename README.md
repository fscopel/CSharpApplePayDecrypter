# How to decrypt Apple Pay Payment Token using .net

This project covers steps 3, and 4 of the Apple documentation about how to decrypt Apple Pay.
Showed here:
https://developer.apple.com/library/archive/documentation/PassKit/Reference/PaymentTokenJSON/PaymentTokenJSON.html

You will need a few things to make this work:
1. You will need the certificate.p12 (name the .p12 file whatever you want) exported from the mac's Keychain Access
    * There are many web sites showing you how to do this online so I wont cover it here in detail    
    * But basically you create a certificate signing request on your mac, and upload it to the Apple developer web site    
    * You will download the generated Apple Pay Certificate and install (simply click only) on your mac    
    * You will open the Keychain Access and export the .p12 file (I did not add a password when exporting the .p12 for this code)

2. You will use openSSL.exe to create the privateKey.der file needed
    * Download and install OpenSSL - I downloaded it from here (windows version): https://slproweb.com/products/Win32OpenSSL.html
    * I used "Win64 OpenSSL v1.1.0L"
    * run the following commands:
      * This will create a sample.key file
      * ```openssl pkcs12 -in certificate.p12 -nocerts -nodes -out sample.key ```
      * This will use the sample.key file and create the privateKey.der file that you need
      * ```openssl pkcs8 -topk8 -inform PEM -outform DER -in sample.key -out privateKey.der -nocrypt```

3. You will need the payment token you are trying to decrypt. 
    * This is the object returned by apple in its entirety.

4. Place the files in whatever directories you are want.
    * The sample app is pointing to the c:\temp directory
    ```C#
    static readonly string paymentToken = @"C:\temp\payment.json";
    static readonly string privateKey = @"C:\temp\privateKey.der";
    static readonly string p12FileLocation = @"C:\temp\certificate.p12";
    ```

If you have any issues please create a bug so we can help others with the same problem.

Thank you.
Happy Coding!
    
      
    
