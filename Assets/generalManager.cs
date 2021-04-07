using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class generalManager : MonoBehaviour
{
    [Header("for signup")]
    public InputField email,password;
    private string emailstring, passstring;
    Firebase.Auth.FirebaseAuth auth;

    [Header("for signin")]
    public InputField email1, password1;
    private string emailstring1, passstring1;

    public Text[] text;
    bool cehcktext = false;
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                var app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (cehcktext)
        {
            display();
            cehcktext = false;
        }
    }

    public void signup()
    {
        emailstring = email.text.ToString();
        passstring = password.text.ToString();
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(emailstring, passstring).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    public void signin()
    {
        emailstring1 = email1.text.ToString();
        passstring1 = password1.text.ToString();
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
    Firebase.Auth.EmailAuthProvider.GetCredential(emailstring1, passstring1);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
        cehcktext = true;
    } 
    void display()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            text[0].text= user.DisplayName;
            text[1].text = user.Email;
            //System.Uri photo_url = user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            text[2].text = user.UserId;
        }
    }
    public void signout()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        Debug.Log("Signed out");
    }
}
