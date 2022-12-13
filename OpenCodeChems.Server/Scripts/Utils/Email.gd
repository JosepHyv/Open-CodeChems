extends Control

func _ready():
	pass

static func SendEmail(emailto, subject, body):
	var emailfrom = "codechems@gmail.com"
	print("generating email")
	var command_body = [	"$EmailFrom = '%s'" %[emailfrom],
	"$EmailTo = '%s'" %[emailto],
	"$Subject = '%s'"%[subject],
	"$Body = '%s'" %[body],
	"$SMTPServer = 'smtp.gmail.com'",
	"$SMTPClient = New-Object Net.Mail.SmtpClient($SmtpServer, 587)",
	"$SMTPClient.EnableSsl = $true",
	"$SMTPClient.Credentials = New-Object System.Net.NetworkCredential('codechems@gmail.com', 'cmoyfycjuwexfawp')",
	"$SMTPClient.Send($EmailFrom, $EmailTo, $Subject, $Body)",
]

	var commands = ""
	var count = 1
	for command in len(command_body):
		if count != len(command_body):
			commands += command_body[command] + "; "
		else:
			commands += command_body[command]
	
		count += 1

	var output = []
	OS.execute("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", [commands], true, output)

