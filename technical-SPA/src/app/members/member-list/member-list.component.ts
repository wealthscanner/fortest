import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_service/alertify.service';
import { UserService } from '../../_service/user.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList: any = [];
  userParams: any = {};
  pagination: Pagination;

  constructor(private userService: UserService,
    private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.setQueryParameters();

    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  setQueryParameters() {

    // show all older than updated today
    this.userParams.OlderThanDays = 0;

    // select gender
    this.userParams.Gender = this.user.gender;

    this.genderList = [{value: 'familyAsset', display: 'FamilyAsset'},
      {value: 'collection', display: 'Account'}];

    if (this.userParams.Gender !== 'collection') {
      this.genderList.push({value: 'personal', display: 'Personal'});
    }
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginatedResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
//    this.setQueryParameters();
  }

}
