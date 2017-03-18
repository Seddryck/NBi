if exist "Extensions" rd "Extensions" /S /Q
md "Extensions"
cd "Extensions"
git clone git://github.com/Seddryck/genbil-tmbundle genbil
cd ..