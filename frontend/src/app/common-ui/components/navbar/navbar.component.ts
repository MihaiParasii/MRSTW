import { Component, inject, OnInit } from '@angular/core'
import { RouterLink } from '@angular/router'
import { NgForOf, NgIf, UpperCasePipe } from '@angular/common'
import { TranslatePipe, TranslateService } from '@ngx-translate/core'

@Component({
	selector: 'app-navbar',
	imports: [
		RouterLink,
		UpperCasePipe,
		NgForOf,
		NgIf,
		TranslatePipe,
	
	],
	templateUrl: './navbar.component.html',
	standalone: true,
	styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
	ngOnInit(): void {
		console.log("item", localStorage.getItem('isLoggedIn'))
	}
	
	private translateService: TranslateService = inject(TranslateService)
	protected currentLanguage: string = 'ro'
	protected languages: string[] = ['ro', 'ru', 'en']
	protected isLoggedIn = localStorage.getItem('isLoggedIn')
	
	public translateText(lang: string) {
		this.currentLanguage = lang
		this.translateService.use(lang)
	}
	
	onLogout() {
		localStorage.setItem('isLoggedIn', '0')
	}
}
