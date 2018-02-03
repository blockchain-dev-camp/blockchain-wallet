if EXIST "%DEPLOYMENT_TARGET%\bower.json" (
    pushd "%DEPLOYMENT_TARGET%"
    call :ExecuteCmd bower install
    IF !ERRORLEVEL! NEQ 0 goto error
    popd
)