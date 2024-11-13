mkdir certificates
cd certificates

mkdir root_ca
openssl genrsa -out root_ca/root_key.pem 4096
openssl req -x509 -new -nodes -key root_ca/root_key.pem \
    -sha256 -days 1024 -out root_ca/root_cert.pem \
    -subj "/O=Qwitter/OU=Qwitter Root CA"


mkdir jwt
openssl genrsa -out jwt/jwt_key.pem 4096
openssl req -new \
    -key jwt/jwt_key.pem \
    -out jwt/csr.csr \
    -subj "/O=Qwitter/OU=JWT"
openssl x509 -req -in jwt/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out jwt/jwt_cert.pem
openssl pkcs12 -export -in jwt/jwt_cert.pem -inkey jwt/jwt_key.pem -out jwt/jwt_cert.pfx -passout pass:password


mkdir user
openssl genrsa -out user/user_key.pem 4096
openssl req -new \
    -key user/user_key.pem \
    -out user/csr.csr \
    -subj "/O=Qwitter/OU=User"
openssl x509 -req -in user/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out user/user_cert.pem
openssl pkcs12 -export -in user/user_cert.pem -inkey user/user_key.pem -out user/user_cert.pfx -passout pass:password


mkdir virtualcrypto
openssl genrsa -out virtualcrypto/virtualcrypto_key.pem 4096
openssl req -new \
    -key virtualcrypto/virtualcrypto_key.pem \
    -out virtualcrypto/csr.csr \
    -subj "/O=Qwitter/OU=Virtual Crypto"
openssl x509 -req -in virtualcrypto/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out virtualcrypto/virtualcrypto_cert.pem
openssl pkcs12 -export -in virtualcrypto/virtualcrypto_cert.pem -inkey virtualcrypto/virtualcrypto_key.pem -out virtualcrypto/virtualcrypto_cert.pfx -passout pass:password


mkdir crypto
openssl genrsa -out crypto/crypto_key.pem 4096
openssl req -new \
    -key crypto/crypto_key.pem \
    -out crypto/csr.csr \
    -subj "/O=Qwitter/OU=Crypto"
openssl x509 -req -in crypto/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out crypto/crypto_cert.pem
openssl pkcs12 -export -in crypto/crypto_cert.pem -inkey crypto/crypto_key.pem -out crypto/crypto_cert.pfx -passout pass:password


mkdir bankaccounts
openssl genrsa -out bankaccounts/bankaccounts_key.pem 4096
openssl req -new \
    -key bankaccounts/bankaccounts_key.pem \
    -out bankaccounts/csr.csr \
    -subj "/O=Qwitter/OU=Bank Accounts"
openssl x509 -req -in bankaccounts/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out bankaccounts/bankaccounts_cert.pem
openssl pkcs12 -export -in bankaccounts/bankaccounts_cert.pem -inkey bankaccounts/bankaccounts_key.pem -out bankaccounts/bankaccounts_cert.pfx -passout pass:password


mkdir exchange
openssl genrsa -out exchange/exchange_key.pem 4096
openssl req -new \
    -key exchange/exchange_key.pem \
    -out exchange/csr.csr \
    -subj "/O=Qwitter/OU=Exchange"
openssl x509 -req -in exchange/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out exchange/exchange_cert.pem
openssl pkcs12 -export -in exchange/exchange_cert.pem -inkey exchange/exchange_key.pem -out exchange/exchange_cert.pfx -passout pass:password

mkdir funds
openssl genrsa -out funds/funds_key.pem 4096
openssl req -new \
    -key funds/funds_key.pem \
    -out funds/csr.csr \
    -subj "/O=Qwitter/OU=Funds"
openssl x509 -req -in funds/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out funds/funds_cert.pem
openssl pkcs12 -export -in funds/funds_cert.pem -inkey funds/funds_key.pem -out funds/funds_cert.pfx -passout pass:password