var mongoose = require('mongoose'),
  Schema = mongoose.Schema;

var PreferencesSchema = new Schema({

	preferences: [String];

});


var Preferences = mongoose.model('Preferences', PreferencesSchema);

module.exports = Preferences;

