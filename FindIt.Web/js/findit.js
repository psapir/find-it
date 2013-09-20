findIt = {
		
	templates: {
		result:null,
	},

	searchIt:function(){
		var typeCount = 0;
		var searchTypes = new Array();
		
		if ($('#checkbox-email').is(':checked'))
		{
			searchTypes[typeCount] = "email";
			typeCount++;
		}
		if ($('#checkbox-contentarea').is(':checked'))
		{
			searchTypes[typeCount] = "content";
			typeCount++;
		}
		if ($('#checkbox-portfolio').is(':checked'))
		{
			searchTypes[typeCount] = "media";
			typeCount++;
		}
		if ($('#checkbox-dataextension').is(':checked'))
		{
			searchTypes[typeCount] = "dataextension";
			typeCount++;
		}

		var findStuff = {
                "mid": 123412,
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
                "customerKey":$('#Customer-Key').is(':checked'),
                "caseSensitive":$('#Case-Sensitive').is(':checked'),
                "pageNumber": 1,
                "itemsPerPage": 20,
                "type": searchTypes,
                "operator": "AND"
            };
		
	    $.post("http://localhost:57259/api/findit/find", findStuff, function () {
		}).done(function () {
		    alert('done!');
		}).fail(function () {
		    alert('failed!');
		});

		/*$.ajax({
			  type: "POST",
			  url: "http://localhost:57259/api/findit/find",
			  data: findStuff,
			  success: function( data ) {
					data = findIt.jsonify(data);
					var results = findIt.templates.result(data);
					$("#myresults").html(results);
				},
			  dataType: "text"
			});*/

            return false;
	},
	
	jsonify: function (object) {
	  // Make sure our object is not a string
		try{
			object = JSON.parse(object);
		} catch(err) {
			// no error logging so far..., but if this occurs, than the object is already a json object
			alert("you have an error");
		}
		
		return object;
	},
	
  init: function () {
    // Don't cache any ajax calls
    $.ajaxSetup({cache: false});
    
    findIt.templates.result = Handlebars.compile($("#resultRow").html());
    
    $("#find-it").click(findIt.searchIt);
  }
};

// load the app

$(document).ready(findIt.init);

Handlebars.registerHelper('list', function(items, options) {
	var out = "";
	  for(var i=0, l=items.length; i<l; i++) {
	    out = out + options.fn(items[i]);
	  }
	  return out;
	});


Handlebars.registerHelper('tolowercase', function(options) {
  return options.fn(this).toLowerCase();
});