import React, { Component } from 'react';
import axios from 'axios'

export class Token extends Component {
  static displayName = Token.name;

  constructor(props) {
    super(props);
    this.state = { username: '', password: '' };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleUsername = this.handleUsername.bind(this);
    this.handlePassword = this.handlePassword.bind(this);
    this.handlePing = this.handlePing.bind(this);
  }

  async login(username, password) {
    const response = await axios.post('login', {
      username,
      password
    });

    if (response.status === 200) {
      const token = response.data;
      sessionStorage.setItem('token', token);
    }
  }

  async handleSubmit(event) {
    event.preventDefault();
    await this.login(this.state.username, this.state.password)
  }

  async ping() {
    const response = await axios.get('ping');

    if (response.status === 200 && response.data === "pong") {
        alert('auth is ok.');
    }
  }

  handleUsername(event) {
    this.setState({username: event.target.value});
  }

  handlePassword(event) {
    this.setState({password: event.target.value});
  }

  async handlePing(event) {
      await this.ping();
  }


  render() {
    return (
        <div class="row">
            <div class="col">
                <form onSubmit={this.handleSubmit}>
                    <label>
                    Username:
                    <input value={this.state.username} onChange={this.handleUsername} />
                    </label>
                    <label>
                    Password:
                    <input value={this.state.password} onChange={this.handlePassword} />
                    </label>
                    <input type="submit" value="Gönder" />
                </form>
            </div>
            <div class="col">
                <a href="javascript:void(0)" onClick={this.handlePing}>Ping</a>

            </div>
        </div>
    );
  }
}
