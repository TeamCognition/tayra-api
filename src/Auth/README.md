Login example:

URL: base + '/connect/token'

HEADER:

[
{"key":"Content-Type","value":"application/x-www-form-urlencoded"},
{"key":"Authorization","value":"Basic dHdjOjlDMTc3MTBBLTg5QzAtNEQ5MS1BOTQ1LTlBRUM5RjA3MUVFMQ=="}
]

BODY:

[
{"key":"grant_type","value":"password"},
{"key":"username","value":"haris@gmail.com"},
{"key":"password","value":"pwd"},
{"key":"resource","value":"https://localhost:5000"}
{"key":"scope","value":"offline_access tAPI"}
]