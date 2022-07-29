<template>
  <GoogleLogin
    class="login-button"
    :params="params"
    :renderParams="renderParams"
    :onSuccess="onSuccess"
  ></GoogleLogin>
</template>

<style #scoped>
.login-button {
  top: 50%;
  left: 50%;
  position: absolute;
  transform: translate(-50%, -50%);
}
</style>

<script>
import GoogleLogin from "vue-google-login";
export default {
  name: "login_signup_social",
  data() {
    return {
      params: {
        client_id:
          "{{CLIENT_ID_GOOGLE}}",
      },
      renderParams: {
        width: 250,
        height: 50,
        longtitle: true,
      },
    };
  },
  components: {
    GoogleLogin,
  },
  methods: {
    onSuccess(googleUser) {      
      const userInfo = { email: googleUser.getBasicProfile().getEmail(), idToken: googleUser.getAuthResponse().id_token };

      localStorage.setItem('loggedUser', JSON.stringify(userInfo))
      console.log(userInfo);
      this.$router.push("/");
    },
  },
};
</script>

<style>
</style>