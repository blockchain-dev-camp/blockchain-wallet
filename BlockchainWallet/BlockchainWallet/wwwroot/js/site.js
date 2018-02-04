// Write your JavaScript code.

window.addEventListener('load', function() {
    $('.hide-on-click').click(hideMe);

    function hideMe() {
        $(this).hide();
    }
});