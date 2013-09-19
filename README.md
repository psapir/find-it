Find it!
=======

Find it! adds search  functionality within ExactTarget.  No matter where or what you need to locate, Find it! finds your stuff in a matter of seconds.

API Documentation
----

### Methods ###
#### Find ####

Main REST API Method

#### Example request ####

```
POST https://www.findit-app.com/find/
{
    "mid" : 123412,
    "keyword": "My Email",
    "pageNumber": 1,
    "itemsPerPage": 20,
    "type": [
             {"typeName" : "Email"},
             {"typeName" : "DataExtension"}
            ],
    "opreator": "AND"
}
```

#### Example response ####

```
HTTP/1.1 202 Accepted
{
    "Results":
    "totalCount":300,
    "page":1,
    "itemsPerPage": 20,
    "items":[
    {
        "name": "Email",
        "customerKey": "Email",
        "type": "Email",
        "URL":"",
        "Path":"my Emails/2013/Dic",
        "ThumbnaiURL":""
    },
    {
        "name": "Email DE",
        "customerKey": "DEEmail",
        "type": "DataExtension",
        "URL":"",
        "Path":"Data Extensions/2013/Dic",
        "ThumbnaiURL":""
    }
}
```
