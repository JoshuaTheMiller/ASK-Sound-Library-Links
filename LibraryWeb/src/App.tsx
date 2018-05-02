import * as React from "react";
import "./App.css";
import SoundList from "./SoundList";

import { Col, Grid, Row } from "react-bootstrap";

import logo from "./logo.svg";

import { Container } from "react-bootstrap/lib/Tab";

export default class App extends React.Component {
  public render() {
    return (
      <Container>
        <Grid className="App">
          <header className="App-header">
            <img src={logo} className="App-logo" alt="logo" />
            <h1 className="App-title">
              <a
                href="https://developer.amazon.com/docs/custom-skills/ask-soundlibrary.html"
                target="_blank"
              >
                Alexa Skills Kit Sound Library Links
              </a>
            </h1>
            <p>
              Disclaimer: This is in no way sponsored by or affiliated with
              Amazon. I just wanted a searchable list of sounds. Additionally,
              do to certain restrictions, audio files are unabled to be
              previewed on this page.
            </p>
          </header>
          <hr />
          <Row>
            <Col>
              <SoundList />
            </Col>
          </Row>
          <hr />
          <footer>
            <p>
              Icon created by{" "}
              <a href="https://www.shareicon.net/aws-101930" target="_blank">
                Aha-Soft
              </a>{" "}
              and shared under the{" "}
              <a
                href="https://creativecommons.org/licenses/by/3.0/"
                target="_blank"
              >
                Creative Commons (Attribution 3.0 Unported)
              </a>{" "}
              license.
            </p>
            <p>
              Check out the project on{" "}
              <a
                href="https://github.com/TheFlyingCaveman/ASK-Sound-Library-Links"
                target="_blank"
              >
                GitHub
              </a>
            </p>
          </footer>
        </Grid>
      </Container>
    );
  }
}
