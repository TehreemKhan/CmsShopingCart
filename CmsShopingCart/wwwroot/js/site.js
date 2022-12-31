﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    if ($("a.confirmDeletion").length) {

        $("a.confirmDeletion").click(() => {
            if (!confirm("confirmDeletion")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setInterval(() => {
            $("div.alert.notification").fadeOut();
        },200);
    }
})
