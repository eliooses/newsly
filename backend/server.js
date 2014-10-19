var restify = require('restify');
var cheerio = require('cheerio');
var mongoose = require('mongoose');
var db = mongoose.connection;
var request = require('request');
var unirest = require('unirest');

db.on('error', console.error);
db.once('open', function() {
	// Create your schemas and models here.
	var ArticleSchema = new mongoose.Schema({
		id: String,
		picurl: String,
		headline: String,
		trailtext: String,
		url: String,
		body: String
	});

	Article = mongoose.model('Article', ArticleSchema);

	var PreferenceSchema = new mongoose.Schema({
		key: String
	});

	Preference = mongoose.model('Preference', PreferenceSchema);

});

mongoose.connect('mongodb://localhost/test');


function saveNewsToDb(obj){
	$ = cheerio.load(obj.fields.trailText);
	var trail = $(obj.fields.trailText).text();
	var article = new Article({
		id: obj.id,
		picurl: extractImageURI(obj.fields.main),
		headline: obj.fields.headline,
		trailtext: trail,
		url: obj.webUrl,
		body: obj.fields.body
	});

	article.save(function(err, article) {
		if (err) return console.error(err);
		console.dir(article);
	});
}

function getAllNews(req,res){
	var data = request('http://content.guardianapis.com/search?api-key=t3myqd7scnfu4t5w8zp7jx4v&show-fields=headline,trailText,main,body&page-size=10', function (error, response, body) {
		if (!error && response.statusCode == 200) {
			var jsonData = JSON.parse(body);
			console.log(jsonData.response.results);
			for (var i = 0; i < jsonData.response.results.length; i++){
				saveNewsToDb(jsonData.response.results[i]);
			}
			//saveNewsToDb(jsonData.response.results);
			res.send(jsonData.response.results);
		}
	})
}

/*function getPrefNews(req,res){
	var data = request('http://content.guardianapis.com/search?api-key=t3myqd7scnfu4t5w8zp7jx4v&show-fields=headline,trailText,main&page-size=10', function (error, response, body) {
		if (!error && response.statusCode == 200) {
			var jsonData = JSON.parse(body);
			console.log(jsonData.response.results);
			for (var i = 0; i < jsonData.response.results.length; i++){
				saveNewsToDb(jsonData.response.results[i]);
			}
			//saveNewsToDb(jsonData.response.results);
			res.send(jsonData.response.results);
		}
	})
}*/


function addPrefs(req, res){
	var incomingID = req.params.id;
	Article.findOne( {_id: incomingID}, function(err, data) {
		if (err) return console.error(err);
		res.send(data);
		var prefs = getKeywords(data.body);
		var mypref = prefs[Math.random() * (prefs.length - 1)];

		var preference = new Preference({
			key:mypref
		});

		preference.save(function(err, preference){
			if (err) console.error(err);
			console.dir(preference);
		});
		// Preferences.findOne add prefs[0]

	});
	//take in id
	//query for body of article from id
	//use get keywords
	// store in preferences

}





function getKeywords(req, res){
	unirest.post("https://joanfihu-article-analysis-v1.p.mashape.com/text")
	.header("X-Mashape-Key", "2LKLhCuMs2mshs6s3OxvtL2325czp1JNAz8jsnz6QtbmGesuEv")
	.header("Content-Type", "application/x-www-form-urlencoded")
	.field("text", "<p>A Roman Catholic Italian missionary order has paid out Â£120,000 to 11 former trainee priests following allegations of widespread sexual abuse at St Peter Claver College in Yorkshire during the 1960s and 70s.</p> <p>The men, most of whom have waived their right to anonymity, say they wanted acknowledgment rather than money and that the order, the Verona Fathers, must now be held accountable for the alleged abuse and its subsequent failure to act. Most were teenagers at the time of the abuse, but the youngest was 11.</p> <p>Four main abusers are named in the menâ€™s statements, although others are identified in corroborating statements from other victims.</p> <p>Two, Fr John Pinkman, the seminaryâ€™s junior housemaster, and Fr Domenico Valmaggia, its infirmarian, are now dead. One, Fr Romano Nardo, lives in the orderâ€™s mother house in Verona.</p> <p>One of the victims, Mark Murray, was recruited into Nardoâ€™s â€œGod Squadâ€, which was addressed on flagellation and the evils of sex.</p> <p>Murray was encouraged by Nardo to wash or â€œpurifyâ€ his naked body, ejaculating into a sink while Murray sponged him. He also showed Murray a cross he had carved on his chest, then used his fingernail to scrape one on Murrayâ€™s. â€œPain,â€ Nardo told him, â€œbrings you closer to God.â€</p>")
	.field('title', 'hi')
	.end(function (result) {	
		console.log(result.status, result.headers, result.body);
		res.send(result.body.keywords);
	});
}


function extractImageURI(main){
	$ = cheerio.load(main);
	var imgurl = $('figure img').attr('src');
	imgurl = imgurl.substring(0, imgurl.length);
	return imgurl;
}

var server = restify.createServer();
server.get('/getallnews', getAllNews);
server.get('/getkeywords', getKeywords);
server.get('/addPrefs/:id', addPrefs)
//server.get('');



server.listen(8080, function() {
  console.log('%s listening at %s', server.name, server.url);
});



