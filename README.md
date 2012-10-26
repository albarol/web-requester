## Web-Requester - Simple API to manipulate requests

Authors: Alexandre Barbieri (fakeezz)

### Examples

#### GET, POST, PUT, DELETE

```c#
var uri = "http://localhost";
var response = webRequester.Get(uri);
```

```c#
var uri = "http://localhost";
var parameters = new { name = "value" };
var headers = new { name = "value" };
var response = webRequester.Put(uri, parameters, headers);
```

#### Download

```c#
var uri = "http://localhost";
var response = webRequester.Download(uri);
File.Move(response.TemporaryFile, "you_path.ext");
```

#### Upload

```c#
var uri = "http://localhost";
var fileStream = new System.IO.FileStream("your_file.ext");
var response = webRequester.Upload(uri, fileStream);
```

