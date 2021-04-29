﻿/// <binding AfterBuild='default' />
var gulp = require("gulp");
var uglify = require("gulp-uglify");
var concat = require("gulp-concat")

function minify() {
	return gulp.src(["wwwroot/js/**/*.js"])
		.pipe(uglify())
		.pipe(concat("dutchtreat.min.js"))
		.pipe(gulp.dest("wwwroot/dist/"));
}

function styles() {
	return gulp.src(["wwwwroot/css/**/*.css"])
		.pipe(uglify())
		.pipe(concat("dutchtreat.min.css"))
		.pipe(gulp.dest("wwwroot/dist/"));
}

exports.minify = minify;
exports.styles = styles;

exports.default = gulp.parallel(minify, styles);