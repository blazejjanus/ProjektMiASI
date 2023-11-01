import secrets

key = secrets.token_hex(32)
key_string = str(key)

print(key_string)