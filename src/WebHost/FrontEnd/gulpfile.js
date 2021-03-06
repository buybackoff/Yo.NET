'use strict';
// generated on 2014-06-18 using generator-gulp-webapp 0.1.0

var gulp = require('gulp');
var path = require('path');
var fs = require('fs');
var rename = require('gulp-rename');
var templates = require('gulp-angular-templatecache');
var minifyHTML = require('gulp-minify-html');

// load plugins
var $ = require('gulp-load-plugins')();

// inject js
gulp.task('injs', function () {
    var target = gulp.src('app/index.html');
    var sources = gulp.src(['app/modules/**/*.js'], { read: false });

    return target.pipe($.inject(sources, { relative: true }))
      .pipe(gulp.dest('app'));
});

gulp.task('templates', function () {
    var addsrc = require('gulp-add-src');

    gulp.src('app/modules/**/*.html')
      .pipe(minifyHTML({
          quotes: true
      }))
      .pipe(templates('generated_templates.js', {
          root: '/modules'
      }))
      .pipe(addsrc('app/scripts/templates.js'))
      .pipe($.concat('templates.js'))
      .pipe($.uglify())
      .pipe(gulp.dest('.tmp/scripts'));

    //.pipe(rename('generated_templates.js'))
    //.pipe(gulp.dest('.tmp/scripts'));

    //gulp.src([
    //    'app/scripts/templates.js',
    //        '.tmp/scripts/generated_templates.js'
    //])

});


gulp.task('jsx', function () {
    return gulp.src('app/scripts/**/*.jsx')
        .pipe($.react())
        .pipe(gulp.dest('.tmp/scripts'));
});

gulp.task('styles', function () {
    return gulp.src('app/styles/main.css')
        .pipe($.autoprefixer('last 1 version'))
        .pipe(gulp.dest('.tmp/styles'))
        .pipe($.size());
});

gulp.task('scripts', function () {
    return gulp.src(['app/scripts/**/*.js', 'app/modules/**/*.js'])
        //.pipe($.jshint())
        //.pipe($.jshint.reporter(require('jshint-stylish')))
        .pipe($.size());
});

gulp.task('servertypes', function () {
    gulp.src('./typings/server.d.ts').pipe($.clean());

    return gulp.src('../../contracts/**/*.d.ts')
        .pipe($.tap(function (file, t) {
            console.log(path.relative('./typings/', file.path));
            fs.appendFile('./typings/server.d.ts',
                '/// <reference path="' + path.relative('./typings/', file.path) + '" />\n');
        }));
});

gulp.task('html', ['injs', 'styles', 'scripts', 'templates'], function () {
    var jsFilter = $.filter('**/*.js');
    var cssFilter = $.filter('**/*.css');
    var ngAnnotate = require('gulp-ng-annotate');
    var assets = $.useref.assets({ searchPath: '{.tmp,app}' });
    return gulp.src('app/*.html')
        .pipe(assets)
        .pipe(jsFilter)
        .pipe(ngAnnotate())
        //.pipe($.ngmin({ dynamic: false }))
        .pipe($.uglify())
        .pipe(jsFilter.restore())
        .pipe(cssFilter)
        .pipe($.csso())
        .pipe(cssFilter.restore())
        .pipe(assets.restore())
        .pipe($.useref())
        .pipe(gulp.dest('dist'))
        .pipe($.size());
});

gulp.task('images', function () {
    return gulp.src('app/images/**/*')
        .pipe($.cache($.imagemin({
            optimizationLevel: 3,
            progressive: true,
            interlaced: true
        })))
        .pipe(gulp.dest('dist/images'))
        .pipe($.size());
});

gulp.task('fonts', function () {
    gulp.src('app/bower_components/components-font-awesome/**/*.{eot,svg,ttf,woff,otf}')
        .pipe($.flatten())
        .pipe(gulp.dest('dist/fonts'))
        .pipe($.size());
    gulp.src('app/bower_components/bootstrap/**/*.{eot,svg,ttf,woff,otf}')
        .pipe($.flatten())
        .pipe(gulp.dest('dist/fonts'))
        .pipe($.size());
});

gulp.task('extras', function () {
    return gulp.src(['app/*.*', '!app/*.html'], { dot: true })
        .pipe(gulp.dest('dist'));
});

gulp.task('clean', function () {
    return gulp.src(['.tmp', 'dist'], { read: false }).pipe($.clean());
});

gulp.task('build', ['html', 'images', 'fonts', 'extras']);

gulp.task('default', ['clean'], function () {
    gulp.start('build');
});

gulp.task('connect', function () {
    var connect = require('connect');
    var app = connect()
        .use(require('connect-livereload')({ port: 35729 }))
        .use(connect.static('app'))
        .use(connect.static('.tmp'))
        .use(connect.directory('app'));

    require('http').createServer(app)
        .listen(9000)
        .on('listening', function () {
            console.log('Started connect web server on http://localhost:9000');
        });
});

gulp.task('serve', ['connect', 'jsx'], function () {
    require('opn')('http://localhost:9000');
});

// inject bower components
gulp.task('wiredep', function () {
    var wiredep = require('wiredep').stream;

    gulp.src('app/*.html')
        .pipe(wiredep({
            directory: 'app/bower_components'
        }))
        .pipe(gulp.dest('app'));
});

gulp.task('watch', ['connect', 'serve'], function () {
    var server = $.livereload();

    // watch for changes

    gulp.watch([
        'app/*.html',
        '.tmp/styles/**/*.css',
        //'app/scripts/**/*.js',
        '{.tmp,app}/scripts/**/*.js',
        'app/images/**/*'
    ]).on('change', function (file) {
        server.changed(file.path);
    });

    gulp.watch('app/styles/**/*.css', ['styles']);
    gulp.watch('app/scripts/**/*.jsx', ['jsx']);
    gulp.watch('app/scripts/**/*.js', ['scripts']);
    gulp.watch('app/images/**/*', ['images']);
    gulp.watch('bower.json', ['wiredep']);

    gulp.watch('../../contracts/**/*.d.ts', ['servertypes']);
});
