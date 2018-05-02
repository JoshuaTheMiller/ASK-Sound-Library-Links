import * as React from "react";
import "./AudioLibraryEntry.css";

export interface IAudioLibraryEntry {
  Category: string;
  Name: string;
  CategoryUri: string;
  Address: string;
}

export class AudioLibraryEntry implements IAudioLibraryEntry {
  public Category: string;
  public Name: string;
  public CategoryUri: string;
  public Address: string;

  constructor(cat: string, name: string, catUri: string, address: string) {
    this.Name = name;
    this.Category = cat;
    this.CategoryUri = catUri;
    this.Address = address;
  }
}

export default function AudioLibraryEntryComponent({
  Category,
  Name,
  CategoryUri,
  Address
}: IAudioLibraryEntry) {
  return (
    <tr>
      <td>
        <a href={CategoryUri} target="_blank">
          {Name}
        </a>
      </td>
      <td>
        <p>
          {Address}
        </p>
      </td>
    </tr>
  );
}
