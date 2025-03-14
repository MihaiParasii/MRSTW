import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable, Subscriber} from 'rxjs';
import {SubcategoryResponse} from '../../models/subcategory/subcategory-response';
import {CreateSubcategoryRequest} from '../../models/subcategory/create-subcategory-request';
import {UpdateSubcategoryRequest} from '../../models/subcategory/update-subcategory-request';

@Injectable({
  providedIn: 'root'
})
export class SubcategoryService {
  http = inject(HttpClient);

  baseApiUrl = "https://darom.md/"
  testApiUrl = "http://localhost:5079/api/v1"


  private subcategories: Array<SubcategoryResponse> = [
    {Id: 1, UrlId: "subcategory-1", Name: "Subcategory 1", CategoryId: 1, CategoryName: "Category 1"},
    {Id: 2, UrlId: "subcategory-2", Name: "Subcategory 2", CategoryId: 2, CategoryName: "Category 2"},
    {Id: 3, UrlId: "subcategory-3", Name: "Subcategory 3", CategoryId: 3, CategoryName: "Category 3"},
    {Id: 4, UrlId: "subcategory-4", Name: "Subcategory 4", CategoryId: 4, CategoryName: "Category 4"},
    {Id: 5, UrlId: "subcategory-5", Name: "Subcategory 5", CategoryId: 5, CategoryName: "Category 5"},
    {Id: 6, UrlId: "subcategory-6", Name: "Subcategory 6", CategoryId: 6, CategoryName: "Category 6"},
    {Id: 7, UrlId: "subcategory-7", Name: "Subcategory 7", CategoryId: 1, CategoryName: "Category 1"},
    {Id: 8, UrlId: "subcategory-8", Name: "Subcategory 8", CategoryId: 2, CategoryName: "Category 2"},
    {Id: 9, UrlId: "subcategory-9", Name: "Subcategory 9", CategoryId: 3, CategoryName: "Category 3"},
  ]

  get(): Observable<Array<SubcategoryResponse>> {
    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.subcategories);
      observer.complete();
    });
    // return Observable<this.categories>;
    // return this.http.get<Doctor[]>(`${this.testApiUrl}/doctors`)
  }

  getById(id: number) {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
  }

  getByCategoryId(id: number) {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
  }

  create(request: CreateSubcategoryRequest) {
    // return this.http.post(`${this.testApiUrl}/doctor`, {
    //   "Name": request.Name,
    //   "Surname": request.Surname,
    //   "PhotoPath": request.PhotoPath,
    //   "SpecialityId": request.SpecialityId
    // });
  }

  update(request: UpdateSubcategoryRequest) {
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
