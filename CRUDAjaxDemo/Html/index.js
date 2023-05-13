//function validation(){
//	if(document.Formfill.Username.value==""){
//		document.getElementById("result").innerHTML="Enter Username*";
//	}
//}
function validation()
{
	if(document.Formfill.Username.value==""){
		const element = document.getElementById("result");
		element.innerHTML = "Enter Username ! ";
		return false;
	}
	else if(document.Formfill.Username.value.length<5){
        const element = document.getElementById("result");
		element.innerHTML="Enter Atleast six letter*";
		return false;
	}
	else if(document.Formfill.email.value==""){
		const element = document.getElementById("result");
		element.innerHTML = "Enter Email ! ";
		return false;
	}
	else if(document.Formfill.password.value==""){
		const element = document.getElementById("result");
		element.innerHTML = "Enter password ! ";
		return false;
	}
	else if(document.Formfill.password.value.length<6){
       const element = document.getElementById("result");
	   element.innerHTML = "Password must be 6-digits ! ";
	   return false;
	}
	else if(document.Formfill.cpassword.value==""){
		const element = document.getElementById("result");
		element.innerHTML = "Enter comfirm Password! ";
		return false;
	}
	else if(document.Formfill.cpassword.value !== document.Formfill.password.value){
		const element = document.getElementById("result");
		element.innerHTML = "Password does'nt matched! ";
		return false;
	}
	else if(document.Formfill.password.value == document.Formfill.cpassword.value){
      popup.classList.add("open-slide")
	  return false;
	}
	else {
const element = document.getElementById("result");
element.innerHTML = " ";
}

}
var popup = document.getElementById('popup');
function CloseSlide(){
	popup.classList.remove('open-slide')
}