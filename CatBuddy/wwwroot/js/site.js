// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Abrir dropdown com hover
$('.dropdown').hover(function () {
    $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeIn(300);
}, function () {
    $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeOut(300);
});

document.addEventListener("DOMContentLoaded", function () {
    // Recupera a classe de todo o projeto
    var password = document.getElementsByClassName("placeholder-password");
    var email = document.getElementsByClassName("placeholder-email");
    var nome = document.getElementsByClassName("placeholder-name");

    // Percorre todas as classes e insere o placeholder
    for (var i = 0; i < nome.length; i++) {
        nome[i].placeholder = "Insira seu nome aqui";
    }

    // Percorre todas as classes e insere o placeholder
    for (var i = 0; i < email.length; i++) {
        email[i].placeholder = "Insira seu email aqui";
    }

    // Percorre todas as classes e insere o placeholder
    for (var i = 0; i < password.length; i++) {
        password[i].placeholder = "**********";
    };
});

$(document).ready(function () {
    $('.date').mask('00/00/0000', { placeholder: "01/01/2000" });
    $('.time').mask('00:00:00');
    $('.date_time').mask('00/00/0000 00:00:00');
    $('.cep').mask('00000-000');
    $('.phone').mask('0000-0000');
    $('.phone_with_ddd').mask('(00) 0000-0000', { placeholder: "(00) 0000-0000" });
    $('.phone_us').mask('(000) 000-0000');
    $('.mixed').mask('AAA 000-S0S');
    $('.cpf').mask('000.000.000-00', { reverse: true, placeholder: "000.000.000-00" });
    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
    $('.money').mask('000.000.000.000.000,00', { reverse: true });
    $('.money2').mask("#.##0,00", { reverse: true });
    $('.ip_address').mask('0ZZ.0ZZ.0ZZ.0ZZ', {
        translation: {
            'Z': {
                pattern: /[0-9]/, optional: true
            }
        }
    });
    $('.percent').mask('##0,00%', { reverse: true });
    $('.clear-if-not-match').mask("00/00/0000", { clearIfNotMatch: true });
    $('.fallback').mask("00r00r0000", {
        translation: {
            'r': {
                pattern: /[\/]/,
                fallback: '/'
            },
            placeholder: "__/__/____"
        }
    });
    $('.selectonfocus').mask("00/00/0000", { selectOnFocus: true });
});