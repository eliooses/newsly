var mongoose = require('mongoose'),
  Schema = mongoose.Schema;

var ArticleSchema = new Schema({
	id: String,
	picurl: String,
	headline: String,
	trailtext: String,
	url: String;
});


var Article = mongoose.model('Article', ArticleSchema);

module.exports = Article;

