import { Component, inject } from '@angular/core'
import { Router, RouterLink } from '@angular/router'
import { TranslatePipe } from '@ngx-translate/core'
import { FormControl, ReactiveFormsModule } from '@angular/forms'

@Component({
	selector: 'app-login',
	standalone: true,
	imports: [
		RouterLink,
		TranslatePipe,
		ReactiveFormsModule
	],
	templateUrl: './login.component.html',
	styleUrl: './login.component.css'
})
export class LoginComponent {
	private router: Router = inject(Router)
	
	email = new FormControl('')
	password = new FormControl('')
	
	onClicked() {
		const emailValue = this.email.value ?? ''
		const passwordValue = this.password.value ?? ''
		console.log(`email: ${emailValue}, password: ${passwordValue}`)
		
		if (emailValue === 'admin@gmail.com' && passwordValue === 'admin') {
			alert('You are logged in as admin')
			localStorage.setItem('isLoggedIn', '2')
			this.router.navigate(['/'])
			return
		}
		
		if (emailValue.includes(passwordValue)) {
			localStorage.setItem('isLoggedIn', '1')
			this.router.navigate(['/'])
			return
		}
		
		localStorage.setItem('isLoggedIn', '0')
	}
}
