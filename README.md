problem-json-webapi
===================

application/problem+json formatter for WebAPI
http://tools.ietf.org/html/draft-nottingham-http-problem-06

## Install

Install-Package problem-json-webapi

Add `config.UseProblemJsonExceptionFormatter();` while configuring WebAPI

```C#
var config = new HttpConfiguration();
...
config.UseProblemJsonExceptionFormatter();
```

## How to use 

HTTP Accept header should contain application/problem+json type

```
Accept: application/json,application/problem+json
```

## Response

```
HTTP/1.1 403 Forbidden
Content-Length: 4067
Content-Type: application/problem+json; charset=utf-8
Server: Microsoft-IIS/8.0
X-Powered-By: ASP.NET

{
  "type": "about:blank",
  "title": "An error has occurred.",
  "detail": "User with Individual Key ...",
  "instance": "urn:ietf:rfc:draft-nottingham-http-problem-06:x-exception:System.UnauthorizedAccessException",
  "debug": " ... stack trace..."
}
```