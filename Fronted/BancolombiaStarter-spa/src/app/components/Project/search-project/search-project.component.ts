import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ProjectFilter } from 'src/app/model/ProjectFilter';

@Component({
  selector: 'app-search-project',
  templateUrl: './search-project.component.html',
  styleUrls: ['./search-project.component.css']
})
export class SearchProjectComponent implements OnInit {
  searchWord: string;
  
  @Output() searchEvent = new EventEmitter<any>();
  constructor() { }

  ngOnInit() {
    this.clean();
    console.log(this.searchWord);
  }


  search() {
    const searchData: ProjectFilter = {
      searchWord: this.searchWord,
    }; 
    this.searchEvent.emit(searchData);
  }

  clean(){
    this.searchWord = "";
  }

}
