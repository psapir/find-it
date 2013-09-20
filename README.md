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
    "createdDate":
        {
            "startDate": "",
            "endDate": ""
        },
    "modifiedDate":
        {
            "startDate": "",
            "endDate": ""
        }
    "caseSensitive":false,
    "pageNumber": 1,
    "itemsPerPage": 20,
    "type": ["Email","DataExtension"],
    "operator": "AND"
}
```

#### Example response ####

```
HTTP/1.1 202 Accepted
{
    "totalCount":300,
    "page":1,
    "itemsPerPage": 20,
    "items":[
    {
        "name": "Email",
        "customerKey": "Email",
        "type": "Email",
        "createdDate": "",
        "modifiedDate": "",
        "URL":"",
        "Path":"my Emails/2013/Dic",
        "ThumbnaiURL":""
    },
    {
        "name": "Email DE",
        "customerKey": "DEEmail",
        "type": "DataExtension",
        "createdDate": "",
        "modifiedDate": "",
        "URL":"",
        "Path":"Data Extensions/2013/Dic",
        "ThumbnaiURL":""
    }
}
```
