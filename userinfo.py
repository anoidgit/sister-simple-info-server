#encoding: utf-8

import sys
reload(sys)
sys.setdefaultencoding( "utf-8" )

from os.path import exists as filexists
from hashlib import sha1

import corealg

def senc(message):
	global enck
	return sha1(message+enck).hexdigest()

userdt={}

dbfile="userdb.txt"
pwdfile="admin.txt"
encrypkeyfile="enc.txt"

enck=""

def ldict(fname):
	rs={}
	with open(fname) as frd:
		for line in frd:
			tmp=line.strip()
			if tmp:
				tmp=tmp.decode("utf-8")
				if tmp and tmp.find("|||")>0:
					tmp=tmp.split("|||")
					if len(tmp)==2:
						rs[tmp[0]] = tmp[-1]
	return rs

def ldpwd(fname):
	rs=""
	try:
		with open(fname) as frd:
			rs=frd.readline()
			rs=rs.strip()
			rs=rs.decode("utf-8")
			rs=senc(rs)
	except:
		pass
	return rs

def wrtdt(dwrt, fname):
	if dwrt:
		with open(fname,"wb") as fwrt:
			for k,v in dwrt.iteritems():
				tmp=k+"|||"+v+"\n"
				fwrt.write(tmp.encode("utf-8"))

admpwd=""
if filexists(pwdfile):
	admpwd=ldpwd(pwdfile)

if filexists(encrypkeyfile):
	enck=ldpwd(encrypkeyfile)

if filexists(dbfile):
	userdt=ldict(dbfile)

def login(uname, pwd):
	global userdt
	if uname.find("|||")==-1:
		if uname in userdt and senc(pwd) == userdt[uname]:
			return False, corealg.getinfo(uname)
		else:
			return True, "认证失败"
	else:
		return True, "用户名中不允许包含|||"

def update(apwd, uname, pwd):
	global admpwd, dbfile, userdt
	if admpwd and admpwd==senc(apwd):
		if uname.find("|||")==-1:
			if filexists(dbfile):
				userdt=ldict(dbfile)
			if uname:
				if uname in userdt:
					userdt[uname]=senc(pwd)
					wrtdt(userdt,dbfile)
					return False, "用户"+str(uname)+"密码已更新"
				else:
					userdt[uname]=senc(pwd)
					wrtdt(userdt,dbfile)
					return False, "创建用户成功"
			else:
				return False, "更新用户数据库成功"
		else:
			return True, "用户名中不允许包含|||"
	else:
		return True, "授权失败"

def clean(apwd, uname):
	global admpwd, dbfile, userdt
	if admpwd and admpwd==senc(apwd):
		if uname in userdt:
			del userdt[uname]
			wrtdt(userdt,dbfile)
			return False, "用户"+str(uname)+"已移除"
		else:
			return False, "用户"+str(uname)+"未找到"
	else:
		return True, "授权失败"