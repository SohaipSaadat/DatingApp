import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUser } from '../Models/iuser';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private baseUrl = 'https://localhost:7252/api/';

  constructor(private http: HttpClient) {}

  getUsersWithRoles() {
    return this.http.get<IUser[]>(`${this.baseUrl}Admin/UsersWithRoles`);
  }

  updateUsersRoles(userName: string, role: string[]) {
    return this.http.post<string[]>(
      `${this.baseUrl}Admin/EditRole/${userName}?role=${role}`,
      {}
    );
  }
}
