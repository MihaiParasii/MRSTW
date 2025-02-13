import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {CreateCategoryRequest} from '../../models/category/create-category-request';
import {CategoryResponse} from '../../models/category/category-response';
import {UpdateCategoryRequest} from '../../models/category/update-category-request';
import {Observable, Subscriber} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  http = inject(HttpClient);

  baseApiUrl = "https://darom.md/"
  testApiUrl = "http://localhost:5079/api/v1"


  private categories:Array<CategoryResponse> = [
    {Id: 1, Name: "Category 1"},
    {Id: 2, Name: "Category 2"},
    {Id: 3, Name: "Category 3"},
    {Id: 4, Name: "Category 4"},
    {Id: 5, Name: "Category 5"},
    {Id: 6, Name: "Category 6"},
    {Id: 7, Name: "Category 7"},
  ]

  get() : Observable<Array<CategoryResponse>> {
    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.categories);
      observer.complete();
    });
    // return Observable<this.categories>;
    // return this.http.get<Doctor[]>(`${this.testApiUrl}/doctors`)
  }

  getById(id: number) {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
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
