import sys
import base64

def encode_image_to_base64(file_path):
    try:
        with open(file_path, 'rb') as image_file:
            encoded_data = base64.b64encode(image_file.read()).decode('utf-8')
            print(encoded_data)
    except FileNotFoundError:
        print(f"Error: File '{file_path}' not found.")
    except Exception as e:
        print(f"An error occurred: {str(e)}")

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python encode_image_to_base64.py <image_file_path>")
    else:
        image_file_path = sys.argv[1]
        encode_image_to_base64(image_file_path)
