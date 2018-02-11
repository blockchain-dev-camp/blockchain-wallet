
window.addEventListener('load', function () {

    $('#btn-tx-get-balance').click(getBalance);
    //console.log(app.bool.shouldCheckBalance);

    if (app.bool.shouldCheckBalance) {
        getBalance();
    }
    
    function getBalance() {
        
        app.startLogo();

        let userAccount = $('#' + app.id.account).val();

        let data = {
            account: userAccount
        }
        
        app.makeRequest(app.url.getBalance, data, 'POST')
            .then(res => {

                if (res.isSuccess) {

                    //let tempBalance = 56;
                    let tempBalance = res.balance;

                    $('#' + app.id.balance).val(tempBalance);

                    canContinue(tempBalance);
                } else {
                    console.log('error !');
                }

                app.stopLogo();

            })
            .catch(err => {
                app.stopLogo();
                console.log(err);
            });

        function canContinue(balance) {
            
            if (balance && Number(balance) > 0) {
                $('#balance-info-msg').removeClass('text-danger');
                $('#balance-info-msg').hide();
                $('#btn-continue-trans').show();
            } else {
                $('#balance-info-msg').addClass('text-danger');
                $('#balance-info-msg').css('font-size', '1.5rem');
                $('#balance-info-msg').text('No funds available!');
            }
        }
    }
});