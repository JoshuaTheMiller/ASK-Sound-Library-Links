import * as React from "react";
import "./SoundList.css";

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
      <section>
        <p>
          Do to certain restrictions, audio files are unabled to be previewed on
          this page.
        </p>
        <p>
          Search: <input type="text" onChange={this.onSearchTextChanged} />
        </p>
        <table>{wordItems}</table>
      </section>
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
