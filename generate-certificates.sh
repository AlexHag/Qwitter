#!/bin/bash

set -e

mkdir certificates
cd certificates

mkdir root_ca
openssl genrsa -out root_ca/root_key.pem 4096
openssl req -x509 -new -nodes -key root_ca/root_key.pem \
    -sha256 -days 1024 -out root_ca/root_cert.pem \
    -subj "/O=Qwitter/OU=qwitter-root-ca"

mkdir jwt
openssl genrsa -out jwt/jwt_key.pem 4096
openssl req -new \
    -key jwt/jwt_key.pem \
    -out jwt/csr.csr \
    -subj "/O=Qwitter/OU=qwitter-jwt"
openssl x509 -req -in jwt/csr.csr \
    -CA root_ca/root_cert.pem \
    -CAkey root_ca/root_key.pem \
    -CAcreateserial -days 500 -sha256 \
    -out jwt/jwt_cert.pem
openssl pkcs12 -export -in jwt/jwt_cert.pem -inkey jwt/jwt_key.pem -out jwt/jwt_cert.pfx -passout pass:password

gen_certificate () {
    mkdir $1

    openssl genrsa -out $1/$1_api_key.pem 4096
    openssl genrsa -out $1/$1_service_key.pem 4096

    openssl req -new \
        -key $1/$1_api_key.pem \
        -out $1/$1_api_csr.csr \
        -subj "/O=Qwitter/OU=qwitter-$1-api/CN=localhost:51$2"    
    openssl req -new \
        -key $1/$1_service_key.pem \
        -out $1/$1_service_csr.csr \
        -subj "/O=Qwitter/OU=qwitter-$1-service/CN=localhost:52$2"

    openssl x509 -req -in $1/$1_api_csr.csr \
        -CA root_ca/root_cert.pem \
        -CAkey root_ca/root_key.pem \
        -CAcreateserial -days 500 -sha256 \
        -out $1/$1_api_cert.pem
    openssl x509 -req -in $1/$1_service_csr.csr \
        -CA root_ca/root_cert.pem \
        -CAkey root_ca/root_key.pem \
        -CAcreateserial -days 500 -sha256 \
        -out $1/$1_service_cert.pem

    openssl pkcs12 -export -in $1/$1_api_cert.pem -inkey $1/$1_api_key.pem -out $1/$1_api_cert.pfx -passout pass:password
    openssl pkcs12 -export -in $1/$1_service_cert.pem -inkey $1/$1_service_key.pem -out $1/$1_service_cert.pfx -passout pass:password
}

gen_certificate "user" "01"
gen_certificate "bankaccounts" "02"
gen_certificate "funds" "03"
gen_certificate "exchange" "04"
gen_certificate "crypto" "05"
