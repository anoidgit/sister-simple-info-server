#encoding: utf-8

import sys
reload(sys)
sys.setdefaultencoding( "utf-8" )

from flask import Flask, request, render_template, send_from_directory

import userinfo

app = Flask(__name__)

@app.route('/', methods = ['GET'])
def login_form():
	return render_template('home.html', loginfail = True)

@app.route('/', methods = ['POST'])
def info_core():
	uname = request.form['username']
	pwd = request.form['password']
	loginstate, msg = userinfo.login(uname, pwd)
	return render_template('home.html', username = uname, loginfail = loginstate, message = msg)

@app.route('/admin', methods = ['GET'])
def admin_login_form():
	return render_template('admin.html', loginfail = True)

@app.route('/admin', methods = ['POST'])
def admin_info_core():
	apwd = request.form['adminpassword']
	uname = request.form['username']
	pwd = request.form['password']
	loginstate, msg = userinfo.update(apwd, uname, pwd)
	return render_template('admin.html', username = uname, loginfail = loginstate, message = msg)

@app.route('/clean', methods = ['GET'])
def clean_login_form():
	return render_template('clean.html', loginfail = True)

@app.route('/clean', methods = ['POST'])
def clean_info_core():
	apwd = request.form['adminpassword']
	uname = request.form['username']
	loginstate, msg = userinfo.clean(apwd, uname)
	return render_template('clean.html', username = uname, loginfail = loginstate, message = msg)

# send everything from client as static content
@app.route('/favicon.ico')
def favicon():
	return send_from_directory(app.root_path, 'favicon.ico', mimetype='image/vnd.microsoft.icon')

if __name__ == '__main__':
	app.run(port=8000, debug=False, host="0.0.0.0")
