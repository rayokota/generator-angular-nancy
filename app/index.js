'use strict';
var util = require('util'),
    path = require('path'),
    yeoman = require('yeoman-generator'),
    _ = require('lodash'),
    _s = require('underscore.string'),
    pluralize = require('pluralize'),
    asciify = require('asciify');

var AngularNancyGenerator = module.exports = function AngularNancyGenerator(args, options, config) {
  yeoman.generators.Base.apply(this, arguments);

  this.on('end', function () {
    this.installDependencies({ skipInstall: options['skip-install'] });
  });

  this.pkg = JSON.parse(this.readFileAsString(path.join(__dirname, '../package.json')));
};

util.inherits(AngularNancyGenerator, yeoman.generators.Base);

AngularNancyGenerator.prototype.askFor = function askFor() {

  var cb = this.async();

  console.log('\n' +
    '+-+-+-+-+-+-+-+ +-+-+-+-+-+ +-+-+-+-+-+-+-+-+-+\n' +
    '|a|n|g|u|l|a|r| |n|a|n|c|y| |g|e|n|e|r|a|t|o|r|\n' +
    '+-+-+-+-+-+-+-+ +-+-+-+-+-+ +-+-+-+-+-+-+-+-+-+\n' +
    '\n');

  var prompts = [{
    type: 'input',
    name: 'baseName',
    message: 'What is the name of your application?',
    default: 'myapp'
  }];

  this.prompt(prompts, function (props) {
    this.baseName = props.baseName;

    cb();
  }.bind(this));
};

AngularNancyGenerator.prototype.app = function app() {

  this.entities = [];
  this.resources = [];
  this.generatorConfig = {
    "baseName": this.baseName,
    "entities": this.entities,
    "resources": this.resources
  };
  this.generatorConfigStr = JSON.stringify(this.generatorConfig, null, '\t');

  this.template('_generator.json', 'generator.json');
  this.template('_package.json', 'package.json');
  this.template('_bower.json', 'bower.json');
  this.template('bowerrc', '.bowerrc');
  this.template('Gruntfile.js', 'Gruntfile.js');
  this.copy('gitignore', '.gitignore');

  var appDir = _s.capitalize(this.baseName) + '/'
  var binDir = appDir + 'bin/'
  var debugDir = binDir + 'Debug/'
  var modelsDir = appDir + 'Models/'
  var publicDir = appDir + 'Content/'
  this.mkdir(appDir);
  this.mkdir(binDir);
  this.mkdir(debugDir);
  this.mkdir(modelsDir);
  this.mkdir(publicDir);

  this.template('_App.sln', _s.capitalize(this.baseName) + '.sln');
  this.copy('_App/NLog.config', debugDir + 'NLog.config');
  this.copy('_App/packages.config', appDir + 'packages.config');
  this.template('_App/_App.csproj', appDir + _s.capitalize(this.baseName) + '.csproj');
  this.template('_App/_AppModule.cs', appDir + _s.capitalize(this.baseName) + 'Module.cs');
  this.template('_App/_AssemblyInfo.cs', appDir + 'AssemblyInfo.cs');
  this.template('_App/_Bootstrapper.cs', appDir + 'Bootstrapper.cs');
  this.template('_App/_HomeModule.cs', appDir + 'HomeModule.cs');
  this.template('_App/_Main.cs', appDir + 'Main.cs');

  var publicCssDir = publicDir + 'css/';
  var publicJsDir = publicDir + 'js/';
  var publicViewDir = publicDir + 'views/';
  this.mkdir(publicCssDir);
  this.mkdir(publicJsDir);
  this.mkdir(publicViewDir);
  this.template('public/_index.html', publicDir + 'index.html');
  this.copy('public/css/app.css', publicCssDir + 'app.css');
  this.template('public/js/_app.js', publicJsDir + 'app.js');
  this.template('public/js/home/_home-controller.js', publicJsDir + 'home/home-controller.js');
  this.template('public/views/home/_home.html', publicViewDir + 'home/home.html');
};

AngularNancyGenerator.prototype.projectfiles = function projectfiles() {
  this.copy('editorconfig', '.editorconfig');
  this.copy('jshintrc', '.jshintrc');
};
