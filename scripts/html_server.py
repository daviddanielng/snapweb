def main():
    from http.server import HTTPServer, SimpleHTTPRequestHandler
    import os

    os.chdir("html")  # Change to the directory containing your HTML files
    server_address = ("", 8000)
    httpd = HTTPServer(server_address, SimpleHTTPRequestHandler)
    print("Serving on http://localhost:8000")

    httpd.serve_forever()


if __name__ == "__main__":
    main()
