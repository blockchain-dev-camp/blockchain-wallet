
window.addEventListener('load', function () {

    $('#btn-tx-get-balance').click(getBalance);
    //console.log(app.bool.shouldCheckBalance);

    if (app.bool.shouldCheckBalance) {
        getBalance();
    }
    
    function getBalance() {
        // 9a9f082f37270ff54c5ca4204a0e4da6951fe917

        app.startLogo();

        let userAccount = $('#' + app.id.account).val();

        let data = {
            account: userAccount
        }

        //console.log(data);
        
        app.makeRequest(app.url.getBalance, data, 'POST')
            .then(res => {

                if (res.isSuccess) {
                    console.log('success');
                    $('#' + app.id.balance).val(res.balance);
                    //$('#' + app.id.balance).val(56);
                } else {
                    console.log('error !');
                }

                app.stopLogo();

            })
            .catch(err => {
                app.stopLogo();
                console.log(err);
            });
    }
});