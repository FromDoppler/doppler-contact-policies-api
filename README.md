# Doppler Contact Policies API

Mini-API to allow users to configure Doppler's contact policies.

See [implementation details](./docs/implementation-details.md).

You can test is using [VS Code REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) with [demo.http](./demo.http).

## Environments

### Production

URL: <https://apis.fromdoppler.com/contact-policies/>

The production environment is updated when a new `v1.*` tag is pushed.

### QA

URL: <https://apisqa.fromdoppler.net/contact-policies/>

The QA environment is updated when `main` branch is updated.

### INT

URL: <https://apisint.fromdoppler.net/contact-policies/>

The INT environment is updated when `INT` branch is updated.
