import * as React from "react";

import rawAduioLibrary from "./audioLibrary.json";

import AudioLibraryEntryComponent, {
  AudioLibraryEntry,
  IAudioLibraryEntry
} from "./AudioLibraryEntry";

export interface ISoundListState {
  audioEntries: IAudioLibraryEntry[];
}

export default class SoundList extends React.Component<any, ISoundListState> {
  private readonly originalAudioEntries: AudioLibraryEntry[];

  constructor(props: any) {
    super(props);

    this.originalAudioEntries = rawAduioLibrary.values.map(
      (value: IAudioLibraryEntry) =>
        new AudioLibraryEntry(
          value.Category,
          value.Name,
          value.CategoryUri,
          value.Address
        )
    );

    this.state = {
      audioEntries: this.originalAudioEntries
    };

    this.onSearchTextChanged = this.onSearchTextChanged.bind(this);
    this.onReadFile = this.onReadFile.bind(this);
  }

  public render() {
    const wordItems = this.state.audioEntries.map(entry => {
      return (
        <AudioLibraryEntryComponent
          key={entry.Name}
          Address={entry.Address}
          Category={entry.Category}
          CategoryUri={entry.CategoryUri}
          Name={entry.Name}
        />
      );
    });

    return (
      <div className="soundListControl">        
        <p>
          Search: <input type="text" onChange={this.onSearchTextChanged} />
        </p>
        <div className="table-responsive">
          <table className="table">
            <thead>
              <tr>
                <th scope="col">Name</th>
                <th scope="col">Link</th>
              </tr>
            </thead>
            {wordItems}
          </table>
        </div>
      </div>
    );
  }

  private onSearchTextChanged(
    textChangedEvent: React.ChangeEvent<HTMLInputElement>
  ): void {
    let updatedList = this.originalAudioEntries;

    updatedList = updatedList.filter(entry => {
      return entry.Name.toLowerCase().includes(
        textChangedEvent.target.value.toLowerCase()
      );
    });

    this.setState({ audioEntries: updatedList });
  }

  private onReadFile(err: any, data: any): void {
    "";
  }
}
