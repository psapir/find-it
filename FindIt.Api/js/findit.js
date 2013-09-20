var page = 0;
var totalPages = 0;

findIt = {

    templates: {
        result: null,
    },

    searchIt: function () {
        var typeCount = 0;
        var searchTypes = new Array();

        if ($('#checkbox-email').is(':checked')) {
            searchTypes[typeCount] = "email";
            typeCount++;
        }
        if ($('#checkbox-contentarea').is(':checked')) {
            searchTypes[typeCount] = "content";
            typeCount++;
        }
        if ($('#checkbox-portfolio').is(':checked')) {
            searchTypes[typeCount] = "media";
            typeCount++;
        }
        if ($('#checkbox-dataextension').is(':checked')) {
            searchTypes[typeCount] = "dataextension";
            typeCount++;
        }

        var findStuff = {
            "mid": 12356,
            "keyword": $("#search-input").val(),
            "createdDate":
            {
                "startDate": $("#date-created-from").val(),
                "endDate": $("#date-created-to").val()
            },
            "modifiedDate":
            {
                "startDate": $("#date-modified-from").val(),
                "endDate": $("#date-modified-to").val()
            },
            "customerKey": $('#Customer-Key').is(':checked'),
            "caseSensitive": $('#Case-Sensitive').is(':checked'),
            "pageNumber": page,
            "itemsPerPage": 20,
            "type": searchTypes,
            "operator": "AND"
        };

        $("#results").fadeOut();

       $.ajax({
			  type: "POST",
			  url: "https://ampscripteditor.qa-pd.com/api/findit/find",
			  data: findStuff,
			  success: function (data) {

			      data = findIt.jsonify(data);
			      totalPages = data.totalCount;
					var results = findIt.templates.result(data);
					$("#myresults").html(results);
					$("#results").fadeIn();
				},
			  dataType: "text"
			});

        return false;
    },

    jsonify: function (object) {
        // Make sure our object is not a string
        try {
            object = JSON.parse(object);
        } catch (err) {
            // no error logging so far..., but if this occurs, than the object is already a json object
            alert("you have an error");
        }

        return object;
    },

    init: function () {
        // Don't cache any ajax calls
        $.ajaxSetup({ cache: false });

        findIt.templates.result = Handlebars.compile($("#resultRow").html());

        $("#find-it").click(findIt.searchIt);

        
        var loading  = false; //to prevents multipal ajax loads
        var total_groups = totalPages; //total record group(s)
    
        $(window).scroll(function() { //detect page scroll
        
            if($(window).scrollTop() + $(window).height() == $(document).height())  //user scrolled to bottom of the page?
            {
            
                if (page <= total_groups && loading == false) //there's more data to load
                {
                    loading = true; //prevent further ajax loading
                    $('.animation_image').show(); //show loading image
                    page = page + 1;

                    var typeCount = 0;
                    var searchTypes = new Array();

                    if ($('#checkbox-email').is(':checked')) {
                        searchTypes[typeCount] = "email";
                        typeCount++;
                    }
                    if ($('#checkbox-contentarea').is(':checked')) {
                        searchTypes[typeCount] = "content";
                        typeCount++;
                    }
                    if ($('#checkbox-portfolio').is(':checked')) {
                        searchTypes[typeCount] = "media";
                        typeCount++;
                    }
                    if ($('#checkbox-dataextension').is(':checked')) {
                        searchTypes[typeCount] = "dataextension";
                        typeCount++;
                    }
                    //load data from the server using a HTTP POST request
                    var findStuff = {
                        "mid": 12356,
                        "keyword": $("#search-input").val(),
                        "createdDate":
                        {
                            "startDate": $("#date-created-from").val(),
                            "endDate": $("#date-created-to").val()
                        },
                        "modifiedDate":
                        {
                            "startDate": $("#date-modified-from").val(),
                            "endDate": $("#date-modified-to").val()
                        },
                        "customerKey": $('#Customer-Key').is(':checked'),
                        "caseSensitive": $('#Case-Sensitive').is(':checked'),
                        "pageNumber": page,
                        "itemsPerPage": 20,
                        "type": searchTypes,
                        "operator": "AND"
                    };


                    $.ajax({
                        type: "POST",
                        url: "https://ampscripteditor.qa-pd.com/api/findit/find",
                        data: findStuff,
                        success: function (data) {
                            data = findIt.jsonify(data);
                            var results = findIt.templates.result(data);
                            $("#myresults").append(results);
                            $('.animation_image').hide();
                            loading = false;
                        },
                        dataType: "text"
                    });
                }
            }
        });
    }
};

// load the app

$(document).ready(findIt.init);

Handlebars.registerHelper('list', function (items, options) {
    var out = "";
    for (var i = 0, l = items.length; i < l; i++) {
        out = out + options.fn(items[i]);
    }
    return out;
});


Handlebars.registerHelper('tolowercase', function (options) {
    return options.fn(this).toLowerCase();
});