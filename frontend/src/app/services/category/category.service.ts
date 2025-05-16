import { inject, Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { CreateCategoryRequest } from '../../models/category/create-category-request'
import { CategoryResponse } from '../../models/category/category-response'
import { UpdateCategoryRequest } from '../../models/category/update-category-request'
import { Observable } from 'rxjs'

@Injectable({
	providedIn: 'root'
})
export class CategoryService {
	http = inject(HttpClient)
	
	// baseApiUrl = "https://darom.md/"
	url = 'http://localhost:8080/api/Category/v1'
	
	
	get(): Observable<CategoryResponse[]> {
		return this.http.get<CategoryResponse[]>(`${this.url}`)
	}
	
	getById(id: number): Observable<CategoryResponse> {
		return this.http.get<CategoryResponse>(`${this.url}/${id}`)
	}
	
	create(request: CreateCategoryRequest) {
		// return this.http.post(`${this.testApiUrl}/doctor`, {
		//   "Name": request.Name,
		//   "Surname": request.Surname,
		//   "PhotoPath": request.PhotoPath,
		//   "SpecialityId": request.SpecialityId
		// });
	}
	
	update(request: UpdateCategoryRequest) {
		// return this.http.put(`${this.testApiUrl}/doctors`, {
		//   "Id": request.Id,
		//   "Name": request.Name,
		//   "Surname": request.Surname,
		//   "PhotoPath": request.PhotoPath,
		//   "SpecialityId": request.SpecialityId,
		// });
	}
	
	delete(id: number) {
		// return this.http.delete(`${this.testApiUrl}/doctors?id=${id}`)
	}
}
