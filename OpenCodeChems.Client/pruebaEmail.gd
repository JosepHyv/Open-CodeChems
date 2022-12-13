extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func SendEmail(emailto, subject, body):
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

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
