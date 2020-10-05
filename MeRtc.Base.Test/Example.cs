// using System.IO;
// using System.Threading;
// using System.Threading.Tasks;

// public class ExampleClass
// {
//     public void Main()
//     {
//         using(FileStream fs = File.Create("PathForEncryptedFile"))
//         {
//             Upload("fileName", fs);
//         }
//     }

//     public void Upload(string filePathToUploadAndEncrypt, Stream uploadStream)
//     {
//         using (FileStream stream = File.Open(filePathToUploadAndEncrypt, FileMode.Open))
//         {
//             byte[] buffer = new byte[100];
//             int numOfReadBytes;
//             using (CryptoStream cryptoStream = new CryptoStream(uploadStream, cryptoTransform, CryptoStreamMode.Write))
//             {
//                 while (stream.Position < stream.Length)
//                 {
//                     numOfReadBytes = stream.Read(buffer, 0, buffer.Length);
//                     cryptoStream.Write(buffer, 0, numOfReadBytes);
//                 }
//                 cryptoStream.FlushFinalBlock();
//             }
//         }
//     }

//     public async Task UploadAsync(string filePathToUploadAndEncrypt, Stream uploadStream)
//     {
//         using (FileStream stream = File.Open(filePathToUploadAndEncrypt, FileMode.Open))
//         {
//             byte[] buffer = new byte[100];
//             int numOfReadBytes;
//             using (CryptoStream cryptoStream = new CryptoStream(uploadStream, cryptoTransform, CryptoStreamMode.Write))
//             {
//                 while (stream.Position < stream.Length)
//                 {
//                     numOfReadBytes = await stream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);
//                     await cryptoStream.WriteAsync(buffer, 0, numOfReadBytes, CancellationToken.None);
//                 }
//                 cryptoStream.FlushFinalBlock();
//             }
//         }
//     }
// }