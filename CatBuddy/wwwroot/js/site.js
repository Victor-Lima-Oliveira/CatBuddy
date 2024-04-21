// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Abrir dropdown com hover
$('.dropdown').hover(function () {
    $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeIn(300);
}, function () {
    $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeOut(300);
});
