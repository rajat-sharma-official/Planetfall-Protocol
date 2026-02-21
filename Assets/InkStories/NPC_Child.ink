// NPC_Child.ink
INCLUDE globals.ink

=== child ===
-> child_hub


= child_hub

Child: "Hi... are you here to help?"

+ [1) "Are you okay?"] -> child_about
+ [2) "What happened here?"] -> child_story
+ [3) "I saw someone named Maerlon."] -> child_maerlon
+ [4) "I'll be back."] -> child_leave


= child_about
Child: "I'm scared, but I'm okay... I think."
-> child_hub


= child_story
Child: "I got separated. Everything looks different at night."
-> child_hub


= child_maerlon
Child: "Maerlon? He said he'd wait near the path."

{ met_child == false:
    ~ met_child = true
    Child: "If you see him again... tell him I’m safe."
- else:
    Child: "Please remind him again... I don’t want him to worry."
}

-> child_hub


= child_leave
Child: "Okay... be careful."
-> END